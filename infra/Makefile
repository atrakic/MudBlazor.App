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
	az resource list --resource-group $(rgName)

outputs:
	az deployment group show --name $(rgName) --resource-group $(rgName) --query properties.outputs

test:
	curl -sL https://$(shell az deployment group show --name $(rgName) --resource-group $(rgName) --query properties.outputs.publicIp.value -o t)

clean:
	az group delete --name $(rgName)
