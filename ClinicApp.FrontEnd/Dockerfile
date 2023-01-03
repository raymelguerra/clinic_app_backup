FROM node:16 as build
WORKDIR /usr/local/app

RUN npm cache clean --force

COPY . .

RUN npm install

RUN npm run build --prod

FROM nginx:1.17.1-alpine

COPY --from=build /usr/local/app/dist /usr/share/nginx/html

COPY ./nginx_config/nginx.conf /etc/nginx/nginx.conf