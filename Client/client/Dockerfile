# ---------- build ----------
# Pinned to node:16 because this app is Angular 11, which will not build on
# modern Node. Containerising the build is what lets CI stay on current agents
# without forcing an Angular upgrade.
FROM node:16-alpine AS build
WORKDIR /app

COPY package*.json ./
# The lockfile predates npm 7's stricter peer-dep resolution, hence --legacy-peer-deps.
RUN npm ci --legacy-peer-deps || npm install --legacy-peer-deps

COPY . .
# NOTE: do NOT set --openssl-legacy-provider here. That flag only exists on
# Node 17+ (the workaround for running old Angular on new Node's OpenSSL 3);
# Node 16 ships OpenSSL 1.1.1, doesn't recognise it, and exits with code 9.
# Cap the heap instead - the default is too generous for a constrained build.
ENV NODE_OPTIONS=--max-old-space-size=2048
# angular.json sets outputPath to "../API/wwwroot" (the original setup served the
# SPA from the API project). Override it here rather than editing angular.json,
# so the local dev workflow keeps working exactly as before.
RUN npx ng build --prod --output-path=/app/dist/client

# ---------- runtime ----------
FROM nginx:alpine AS final
# SPA routing: unknown paths must fall back to index.html or a refresh on
# /shop or /orders would 404 at the web-server level.
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/client /usr/share/nginx/html
EXPOSE 80
