using System;
using System.Text;
using System.Collections.Generic;
using Pulumi;
using Aws = Pulumi.Aws;

return await Deployment.RunAsync(() => 
{
   var ubuntu = Aws.Ec2.GetAmi.Invoke(new()
      {
        MostRecent = true,
        Filters = new[]
        {
            new Aws.Ec2.Inputs.GetAmiFilterInputArgs
            {
                Name = "name",
                Values = new[]
                {
                    "ubuntu/images/hvm-ssd/ubuntu-jammy-22.04-amd64-server-*",
                },
            },
            new Aws.Ec2.Inputs.GetAmiFilterInputArgs
            {
                Name = "virtualization-type",
                Values = new[]
                {
                    "hvm",
                },
            },
        },
    });

string script = @"#!/bin/bash
apt-get update -y
apt-get upgrade -y
apt-get install unzip -y

apt-get install \
    ca-certificates \
    curl \
    gnupg \
    lsb-release
mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
echo \
  ""deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable"" | tee /etc/apt/sources.list.d/docker.list > /dev/null
apt-get update -y
apt-get install docker-ce docker-ce-cli containerd.io docker-compose-plugin -y

cd /
mkdir /app && mkdir /app/python

cd /var/tmp
wget https://github.com/guigsgbm/Challenge_Devops_Alura/archive/refs/heads/main.zip
unzip main.zip
cp Challenge_Devops_Alura-main -r /app/python/
rm -r *";

   var web = new Aws.Ec2.Instance("iac-aws-challenge-devops-alura", new()
      { 
        Ami = ubuntu.Apply(getAmiResult => getAmiResult.Id),
        InstanceType = "t2.micro",
        AssociatePublicIpAddress = true,
        KeyName = "iac-alura",
        VpcSecurityGroupIds = new[]
        {
           "sg-06e67842a2d739993",
        },
        UserData = script,
    });
});