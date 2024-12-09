using Amazon.CDK;
using Stack;

var app = new App();

BatchStack.Create(app, "prod");

app.Synth();