#!/bin/bash

# Usage make.sh <kind> <path>

BASE_DIR=`pwd`
TARGET_PATH="$2"
mkdir -p $TARGET_PATH
ABS_TARGET_PATH=`cd $TARGET_PATH; pwd`
cd "$(dirname "$(dirname "$0")")/mason"
mason get
mason make $1 -o $ABS_TARGET_PATH
