FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

RUN set -x \
	&& dotnet tool update --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"

COPY --chmod=0755 entrypoint.sh /usr/local/bin/
ENTRYPOINT ["entrypoint.sh"]
