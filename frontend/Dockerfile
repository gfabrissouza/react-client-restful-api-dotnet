# Etapa 1: Build da aplicação
FROM node:18.17.1 AS build
WORKDIR /app
COPY ["package.json", "package-lock.json", "./"]
RUN npm install
COPY . .
RUN npm run build

# Etapa 2: Servir os arquivos usando `serve`
FROM node:18.17.1 AS production
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/dist ./dist
EXPOSE 3000
CMD ["serve", "-s", "dist", "-l", "3000"]