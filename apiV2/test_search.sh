#!/bin/bash

# BPMM API Search Endpoint Test Script
# Usage: ./test_search.sh [base_url]
# Default base URL: http://localhost:6000

BASE_URL=${1:-"http://localhost:6000"}
API_BASE="$BASE_URL/api/libros"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}BPMM API Search Endpoint Tests${NC}"
echo -e "${BLUE}Testing against: $BASE_URL${NC}"
echo "==============================================="

# Function to test endpoint
test_endpoint() {
    local name="$1"
    local url="$2"
    local expected_status="${3:-200}"
    
    echo -e "\n${YELLOW}Test: $name${NC}"
    echo -e "${BLUE}URL: $url${NC}"
    
    response=$(curl -s -w "\n%{http_code}" "$url")
    status_code=$(echo "$response" | tail -n1)
    body=$(echo "$response" | head -n -1)
    
    if [ "$status_code" -eq "$expected_status" ]; then
        echo -e "${GREEN}✓ Status: $status_code (Expected: $expected_status)${NC}"
        if [ "$status_code" -eq 200 ]; then
            count=$(echo "$body" | jq -r '.count // "N/A"' 2>/dev/null)
            success=$(echo "$body" | jq -r '.success // "N/A"' 2>/dev/null)
            echo -e "${GREEN}✓ Success: $success, Count: $count${NC}"
        fi
    else
        echo -e "${RED}✗ Status: $status_code (Expected: $expected_status)${NC}"
    fi
    
    echo -e "${BLUE}Response:${NC}"
    echo "$body" | jq . 2>/dev/null || echo "$body"
    echo "-----------------------------------------------"
}

# Test 1: Health check
echo -e "\n${YELLOW}=== HEALTH CHECK ===${NC}"
test_endpoint "Health Check" "$BASE_URL/health"

# Test 2: Get categories
echo -e "\n${YELLOW}=== GET CATEGORIES ===${NC}"
test_endpoint "Get Categories" "$API_BASE/categories"

# Test 3: Search without filters (pagination test)
echo -e "\n${YELLOW}=== BASIC SEARCH TESTS ===${NC}"
test_endpoint "Search All (Default pagination)" "$API_BASE/filtered"
test_endpoint "Search with custom pagination" "$API_BASE/filtered?pageNumber=0&pageSize=5"

# Test 4: Search by author
echo -e "\n${YELLOW}=== AUTHOR SEARCH TESTS ===${NC}"
test_endpoint "Search by author (Garc%C3%ADa)" "$API_BASE/filtered?author=Garc%C3%ADa"
test_endpoint "Search by author (partial match)" "$API_BASE/filtered?author=Mar"

# Test 5: Search by title
echo -e "\n${YELLOW}=== TITLE SEARCH TESTS ===${NC}"
test_endpoint "Search by title (Don)" "$API_BASE/filtered?title=Don"
test_endpoint "Search by title (partial)" "$API_BASE/filtered?title=amor"

# Test 6: Search by category
echo -e "\n${YELLOW}=== CATEGORY SEARCH TESTS ===${NC}"
test_endpoint "Search by category (Novela)" "$API_BASE/filtered?categories=Novela"
test_endpoint "Search by category (riojanos)" "$API_BASE/filtered?categories=riojanos"

# Test 7: Combined filters
echo -e "\n${YELLOW}=== COMBINED FILTER TESTS ===${NC}"
test_endpoint "Search by author + title" "$API_BASE/filtered?author=Garc%C3%ADa&title=Cien"
test_endpoint "Search by author + category" "$API_BASE/filtered?author=Garc%C3%ADa&categories=Novela"
test_endpoint "Search all filters" "$API_BASE/filtered?author=Garc%C3%ADa&title=amor&categories=Novela"

# Test 8: Pagination edge cases
echo -e "\n${YELLOW}=== PAGINATION TESTS ===${NC}"
test_endpoint "Large page size" "$API_BASE/filtered?pageSize=50"
test_endpoint "Second page" "$API_BASE/filtered?pageNumber=1&pageSize=10"

# Test 9: Error cases
echo -e "\n${YELLOW}=== ERROR HANDLING TESTS ===${NC}"
test_endpoint "Invalid page number" "$API_BASE/filtered?pageNumber=-1" 400
test_endpoint "Invalid page size (too large)" "$API_BASE/filtered?pageSize=101" 400
test_endpoint "Invalid page size (zero)" "$API_BASE/filtered?pageSize=0" 400
test_endpoint "Non-numeric pagination" "$API_BASE/filtered?pageNumber=abc&pageSize=xyz" 400

# Test 10: URL encoding test
echo -e "\n${YELLOW}=== URL ENCODING TESTS ===${NC}"
test_endpoint "Search with spaces" "$API_BASE/filtered?author=Garc%C3%ADa%20M%C3%A1rquez"
test_endpoint "Search with special characters" "$API_BASE/filtered?title=Cien%20a%C3%B1os"

echo -e "\n${GREEN}=== ALL TESTS COMPLETED ===${NC}"
echo -e "${BLUE}Note: Install 'jq' for better JSON formatting: sudo apt-get install jq${NC}"