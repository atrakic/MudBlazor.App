FROM mcr.microsoft.com/dotnet/sdk:8.0
LABEL org.opencontainers.image.description="dotnet image with ef core tools installed"
WORKDIR /app

RUN set -x \
	&& dotnet tool update --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"
ENV TZ="Etc/UTC"

COPY --chmod=0755 entrypoint.sh /usr/local/bin/
ENTRYPOINT ["entrypoint.sh"]
