		#API MANGEMENT INSTANCE CREATION

#location Name , Resource Group Name , EmailId,API name
myLocation=westus2
myGroup=az204-apim-rg
myEmail=<your email>
myApiName=az204-apim-$RANDOM

#----------------------------------------------------
#Step1:

az group create \
	--name $myGroup \
	--location $myLocation

az apim create \
	--name $myApiName
	--location $myLocation
	--publisher-email $myEmail
	--resouece-group $myGroup
	--publisher-name az204-apim-Test \
	--sku-name Consumption
	
az group delete \
	--name $myGroup