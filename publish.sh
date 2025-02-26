#!/bin/bash
set -x #echo on

BUILD_NUMBER=$(date +%s)
PACKETNAME=bibmamo_$BUILD_NUMBER.tar.gz
DOMAIN=bibliotecamarianomoreno.org
REMOTEDIR=domains/bibliotecamarianomoreno.org/public_html
#REMOTECOMMAND="mkdir -p $REMOTEDIR/../bkp/$BUILD_NUMBER; cp -r $REMOTEDIR $REMOTEDIR/../bkp/$BUILD_NUMBER; rm -r $REMOTEDIR; mkdir $REMOTEDIR;tar -xf $PACKETNAME -C $REMOTEDIR"; 

echo Making Package
cd dist/bibmamo
tar czf ../$PACKETNAME .
echo Copying Package to Server
scp -P 65002  ../$PACKETNAME  u713988895@srv1923.main-hosting.eu:. 

echo Backing up current Copy
ssh -p65002 u713988895@srv1923.main-hosting.eu "rm -r backups/latest/$REMOTEDIR ; mkdir -p backups/latest/$REMOTEDIR; mv $REMOTEDIR backups/latest/$REMOTEDIR"

echo Extracting Package Remotely
ssh -p65002 u713988895@srv1923.main-hosting.eu "mkdir -p $REMOTEDIR; tar -xf $PACKETNAME -C $REMOTEDIR"
