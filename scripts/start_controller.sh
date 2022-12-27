#!/bin/bash

cd "$(dirname "$0")/../controller"

PORT=${1:-8202}

fvm flutter clean
fvm flutter build web
cd build/web
npx http-server -p $PORT
