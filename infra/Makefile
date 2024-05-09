SHELL := /bin/bash

rgName?=exampleRG
adminUsername?=atrakic
location?=northeurope
dnsLabelPrefix?=example-$(shell date +%s)
gitRepo?=https://github.com/atrakic/MudBlazor.App.git
vmName?=demo
sshAuthKey?=$(shell cat $$HOME/.ssh/id_rsa.pub)

.PHONY: all status outputs test clean

all:
	az account show
	az group create --name $(rgName) --location $(location)
	az deployment group create \
		--resource-group $(rgName) \
		--template-file main.bicep \
		--parameters dnsLabelPrefix=$(dnsLabelPrefix) \
		--parameters adminUsername=$(adminUsername) \
		--parameters gitRepo=$(gitRepo) \
		--parameters sshAuthKey=$(sshAuthKey) \
		--parameters vmName=$(vmName) \
		--parameters location=$(location)

status:
	az resource list --resource-group $(rgName)

outputs:
	az deployment group show --name $(rgName) --resource-group $(rgName) --query properties.outputs

test:
	curl -sL https://$(dnsLabelPrefix).northeurope.cloudapp.azure.com

clean:
	az group delete --name $(rgName)
