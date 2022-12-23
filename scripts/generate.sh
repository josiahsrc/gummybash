#!/bin/bash

cd "$(dirname "$0")/../controller"
TYPE=${1-build}
fvm flutter pub run build_runner $TYPE --delete-conflicting-outputs
