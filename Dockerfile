FROM node:12.18.3-alpine3.12

#install angular CLI
RUN npm install -g @angular/cli@10.0.5

WORKDIR /app
COPY package.json .
COPY package-lock.json .
RUN npm install
COPY . .
RUN ng build --prod
RUN npm install -g http-server
EXPOSE 4200
CMD ["http-server", "dist/bibmamo", "-p", "4200"]