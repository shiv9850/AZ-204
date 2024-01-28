#REDIS CHACHE
#location Name , Resource Group Name 
myLocation=westus2
myGroup=az204-redis-rg
redisName=az204-redis-$RANDOM


az group create \
	--name $myGroup \
	--location $myLocation

az redis create \
	--name $redisName \
	--location $myLocation \
	--resource-group $myGroup \
	--sku Basic
	--vm-size c0


az group delete \
	--name $myGroup --no-wait