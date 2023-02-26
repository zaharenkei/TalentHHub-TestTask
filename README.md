# TalentHHub test task

## Problem Statement
At HH Global a "job" is a group of print items.  For example, a job can be a run of business cards, envelopes, and letterhead together.

Some items qualify as being sales tax free, whereas, by default, others are not.  Sales tax is 7%.

HH Global also applies a margin, which is the percentage above printing cost that is charged to the customer.  
For example, an item that costs $100 to print that has a margin of 11% will cost:
item: $100 -> $7 sales tax = $107
job:  $100 -> $11 margin
total: $100 + $7 + $11 = $118

The base margin is 11% for all jobs this problem.  Some jobs have an "extra margin" of 5%.  These jobs that are flagged as extra margin have an additional 5% margin (16% total) applied.

The final cost is rounded to the nearest even cent.
Individual items are rounded to the nearest cent.

Develop a web application (web service) that exposes API to accept jobs in JSON format, calculates the total charge to a customer for a job and returns result in JSON.
(Bonus: try to pack application it to a docker container to be deployed into Kubernetes cluster with a Helm 3 Chart. Note that cluster nodes are running under Ubuntu 18.04)

## Remarks
- I was trying to keep implementation without overcomplicated details.
- Absence of unit tests was intentional, as we could check mock of the flows with integration ones
- Integration tests were selected to be able to check provided samples in one click
- Validation was added, despite the fact we could ingest almost any data set with the $0 result in total cost.
- Logging was configured to console-only, but surely could be expanded.

## Test data
Provided tests data set could be found under test project in IntegrationTests/JobControllerTests.cs file.

## Running with IDE

### Steps

- Download the source code
- Open the solution folder with IDE
- Run API under any service available

## Deploying locally by DockerDesctop & Helm

### Requirements

- DockerDesktop with kubernetes enabled
- Helm installed

### Steps

- Download the source code

- navigate to TalentHHub-TestTask solution directory

Build a docker image for further usage:
- docker image build --pull -t testtask:v1 .

Running it with helm 3 chart:
- run: helm install testrelease ./chart/

Exposing service port for externall access
- run: kubectl port-forward service/testrelease-service 9999:8888
