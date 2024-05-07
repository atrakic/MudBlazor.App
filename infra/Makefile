rgName?=exampleRG
userdata?=$(shell base64 < cloud-init.yaml) 
location?=eastus

all:
	az group create --name $(rgName) --location $(location)
	az deployment group create \
		--resource-group $(rgName) \
		--template-file main.bicep \
		--parameters adminUsername=$$(whoami) \
		--parameters userData=$(USERDATA)

status:
	az resource list --resource-group $(rgName)

clean:
	az group delete --name $(rgName)