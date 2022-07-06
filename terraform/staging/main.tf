provider "aws" {
  region  = "eu-west-2"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
  application_name = "housing-repairs-online-api" # The name to use for your application
}

terraform {
  required_providers {
    aws = {
      source = "registry.terraform.io/hashicorp/aws"
      version = "~> 3.69.0"
    }
  }
  backend "s3" {
    bucket  = "terraform-state-housing-staging"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/housing-repairs-online-api/state"
  }
}

module "dynamodb_table" {
  source = "terraform-aws-modules/dynamodb-table/aws"

  name     = "HousingRepairsOnline"
  hash_key = "id"

  attributes = [
    {
      name = "id"
      type = "S"
    }
  ]
}

