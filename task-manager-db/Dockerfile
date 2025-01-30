FROM alpine:3.18

RUN apk add --no-cache sqlite

WORKDIR /database
COPY scripts/*.sql /scripts/

CMD ["sh", "-c", "\
  if [ ! -f /database/taskmanager.db ]; then \
    echo 'Initializing database...'; \
    sqlite3 /database/taskmanager.db < /scripts/01_create_tables.sql && \
    sqlite3 /database/taskmanager.db < /scripts/02_seed_data.sql; \
  fi && \
  echo 'Database ready' && \
  tail -f /dev/null"]
