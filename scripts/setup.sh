#!/bin/bash

cd "$(dirname "$0")/../"

rm -rf .venv
python3 -m venv .venv
