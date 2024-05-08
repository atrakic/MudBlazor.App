rgName?=exampleRG
adminUsername?=atrakic
location?=westeurope
userdata?=$(shell cat < cloud-init.yml)

all:
	az account show
	az group create --name $(rgName) --location $(location)
	az deployment group create \
		--resource-group $(rgName) \
		--template-file main.bicep \
		--parameters adminUsername=$(adminUsername) \
		--parameters userData=$(userdata)

status:
	az resource list --resource-group $(rgName)

clean:
	az group delete --name $(rgName)
