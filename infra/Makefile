SHELL := /bin/bash

rgName?=rg-vm-demo
adminUsername?=$(shell whoami)
location?=northeurope
vmName?=demo
adminPasswordOrKey?=$(shell cat $$HOME/.ssh/id_rsa.pub)

.PHONY: all status outputs test clean

all:
	az account show
	az group create --name $(rgName) --location $(location)
	az deployment group create \
		--resource-group $(rgName) \
		--template-file main.bicep \
		--parameters adminUsername=$(adminUsername) \
		--parameters adminPasswordOrKey="$(adminPasswordOrKey)" \
		--parameters vmName=$(vmName) \
		--parameters location=$(location)

status:
	az resource list --resource-group $(rgName) --location $(location) --output table

outputs:
	az deployment group list --resource-group $(rgName) --query "[].properties.outputs"

lint:
	bicep lint main.bicep

test:
	curl -sL https://$(shell az deployment group show --name "main" --resource-group $(rgName) --query properties.outputs.hostname.value -o tsv)

healthz:
	curl -sL https://$(shell az deployment group show --name "main" --resource-group $(rgName) --query properties.outputs.hostname.value -o tsv)/healthz

clean:
	az group delete --name $(rgName)
