#!/bin/bash

cd "$(dirname "$0")/../controller"

fvm flutter build web --release
firebase deploy --only hosting
