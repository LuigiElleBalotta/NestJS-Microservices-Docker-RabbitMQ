FROM node:14-alpine AS builder

RUN apk add --update \
  git \
  openssh-client \
&& rm -rf /var/cache/apk/*

WORKDIR "/app"

COPY . .

RUN npm i -f
RUN npm run build
RUN npm prune --production

FROM node:14-alpine AS production

WORKDIR "/app"

COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/package-lock.json ./package-lock.json
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
EXPOSE 3006
CMD [ "sh", "-c", "npm run start:prod"]
