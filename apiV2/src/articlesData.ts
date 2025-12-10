import * as sqlite3 from 'sqlite3';

interface Article {
    articleId?: number;
    handle?: string; // Keep for frontend compatibility but won't be stored
    title: string;
    templateId?: string; // Keep for frontend compatibility but won't be stored
    mainImageUrl?: string;
    htmlContent: string;
    author: string;
    subtitle: string;
    tags?: string;
}

let _articlesDb: sqlite3.Database | null = null;

function getArticlesDb(): sqlite3.Database {
    if (!_articlesDb) {
        _articlesDb = new sqlite3.Database('data/articles.db', (err) => {
            if (err) {
                console.error('Error opening articles database:', err);
                throw err;
            }
        });
        
        // Check if table exists, throw error if it doesn't
        _articlesDb.get(`SELECT name FROM sqlite_master WHERE type='table' AND name='Articles'`, [], (err, row) => {
            if (err) {
                console.error('Error checking Articles table existence:', err);
                throw err;
            }
            if (!row) {
                console.error('Articles table does not exist');
                throw new Error('Articles table does not exist. Please create the table first.');
            }
            console.log('Articles table exists and ready');
        });
        
        // Handle process termination gracefully
        process.on('SIGINT', closeArticlesDb);
        process.on('SIGTERM', closeArticlesDb);
        process.on('exit', closeArticlesDb);
    }
    return _articlesDb;
}

function closeArticlesDb() {
    if (_articlesDb) {
        _articlesDb.close((err) => {
            if (err) {
                console.error('Error closing articles database:', err);
            } else {
                console.log('Articles database connection closed.');
            }
            _articlesDb = null;
        });
    }
}

function getAllArticles(): Promise<Article[]> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        db.all(`SELECT * FROM Articles ORDER BY ArticleId DESC`, [], (err, rows: any[]) => {
            if (err) {
                console.error('Error fetching articles:', err);
                return reject(err);
            }
            const articles: Article[] = rows.map(row => ({
                articleId: row.ArticleId,
                title: row.Title,
                mainImageUrl: row.MainImageUrl,
                htmlContent: row.HtmlContent,
                author: row.Author,
                subtitle: row.Subtitle,
                tags: row.Tags
            }));
            resolve(articles);
        });
    });
}

function getArticleById(id: string): Promise<Article | null> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        db.get(`SELECT * FROM Articles WHERE ArticleId = ?`, [id], (err, row: any) => {
            if (err) {
                console.error('Error fetching article by ID:', err);
                return reject(err);
            }
            if (!row) {
                return resolve(null);
            }
            const article: Article = {
                articleId: row.ArticleId,
                title: row.Title,
                mainImageUrl: row.MainImageUrl,
                htmlContent: row.HtmlContent,
                author: row.Author,
                subtitle: row.Subtitle,
                tags: row.Tags
            };
            resolve(article);
        });
    });
}

function createArticle(article: Omit<Article, 'articleId'>): Promise<Article> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        const stmt = db.prepare(`INSERT INTO Articles 
            (Title, MainImageUrl, HtmlContent, Author, Subtitle, Tags) 
            VALUES (?, ?, ?, ?, ?, ?)`);
        
        stmt.run([
            article.title,
            article.mainImageUrl || null,
            article.htmlContent,
            article.author,
            article.subtitle,
            article.tags || null
        ], function(err) {
            if (err) {
                console.error('Error creating article:', err);
                return reject(err);
            }
            
            // Return the created article
            const createdArticle: Article = {
                ...article,
                articleId: this.lastID
            };
            resolve(createdArticle);
        });
        
        stmt.finalize();
    });
}

function updateArticle(article: Article): Promise<Article> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        const stmt = db.prepare(`UPDATE Articles SET 
            Title = ?, MainImageUrl = ?, HtmlContent = ?, Author = ?, Subtitle = ?, Tags = ?
            WHERE ArticleId = ?`);
        
        stmt.run([
            article.title,
            article.mainImageUrl || null,
            article.htmlContent,
            article.author,
            article.subtitle,
            article.tags || null,
            article.articleId
        ], function(err) {
            if (err) {
                console.error('Error updating article:', err);
                return reject(err);
            }
            
            if (this.changes === 0) {
                return reject(new Error('Article not found'));
            }
            
            resolve(article);
        });
        
        stmt.finalize();
    });
}

function deleteArticle(id: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        db.run(`DELETE FROM Articles WHERE ArticleId = ?`, [id], function(err) {
            if (err) {
                console.error('Error deleting article:', err);
                return reject(err);
            }
            
            if (this.changes === 0) {
                return reject(new Error('Article not found'));
            }
            
            resolve(true);
        });
    });
}

function searchArticlesByTags(tags: string): Promise<Article[]> {
    return new Promise((resolve, reject) => {
        const db = getArticlesDb();
        db.all(`SELECT * FROM Articles WHERE Tags LIKE ? COLLATE NOCASE ORDER BY ArticleId DESC`, 
               [`%${tags}%`], (err, rows: any[]) => {
            if (err) {
                console.error('Error searching articles by tags:', err);
                return reject(err);
            }
            
            const articles: Article[] = rows.map(row => ({
                articleId: row.ArticleId,
                title: row.Title,
                mainImageUrl: row.MainImageUrl,
                htmlContent: row.HtmlContent,
                author: row.Author,
                subtitle: row.Subtitle,
                tags: row.Tags
            }));
            resolve(articles);
        });
    });
}

export { 
    Article, 
    getAllArticles, 
    getArticleById, 
    createArticle, 
    updateArticle, 
    deleteArticle, 
    searchArticlesByTags,
    closeArticlesDb 
};