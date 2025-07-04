
FROM node:20-alpine as build

WORKDIR /app

COPY package*.json ./
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build --configuration=production


FROM nginx:alpine


RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist/upi-payment-app/browser /usr/share/nginx/html


EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
