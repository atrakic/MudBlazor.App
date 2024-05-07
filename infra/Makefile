rgName?=exampleRG
adminUsername?=admin
location?=westeurope
userdata?=$(shell cat < cloud-init.txt)

all:
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
