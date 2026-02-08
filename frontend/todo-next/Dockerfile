# Stage 1: build the React app
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

CMD ["npm", "start"]


# Stage 2: serve with Nginx
#FROM nginx:alpine

#RUN rm /etc/nginx/conf.d/default.conf

#COPY --from=build /app/out /usr/share/nginx/html

#COPY nginx.conf /etc/nginx/conf.d

#EXPOSE 80

#CMD ["nginx", "-g", "daemon off;"]

