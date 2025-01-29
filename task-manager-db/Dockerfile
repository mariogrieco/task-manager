FROM alpine:3.18

RUN apk add --no-cache sqlite

WORKDIR /db
VOLUME /db

COPY scripts/*.sql /scripts/

CMD ["sh", "-c", "\
  if [ ! -f /db/taskmanager.db ]; then \
    echo 'Initializing db...'; \
    sqlite3 /db/taskmanager.db < /scripts/01_CreateTables.sql && \
    sqlite3 /db/taskmanager.db < /scripts/02_SeedData.sql; \
  fi && \
  echo 'DB ready' && \
  tail -f /dev/null"]
