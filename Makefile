
SHELL := bash
.SHELLFLAGS := -eu -o pipefail -c
.ONESHELL:
.DELETE_ON_ERROR:
MAKEFLAGS += --warn-undefined-variables --no-builtin-rules
.RECIPEPREFIX = >

deploy:
> if ! git diff-index --quiet HEAD --; then 
>   echo "Won't publish with uncommited changes"
>   exit
> fi
> dotnet publish --output target/Calories --configuration Release --runtime linux-x64
> tar -czf target/Calories.tgz -C target Calories
> scp target/Blog.tgz calories@nuit:/home/calories
> ssh calories@nuit /home/calories/bin/deploy.sh
.PHONY: deploy

clean:
> -rm -rf target
