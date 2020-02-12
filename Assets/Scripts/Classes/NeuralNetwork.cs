public class NeuralNetwork
{

  public readonly int inputs, outputs, numberOfHiddenLayers, numberOfHiddenNodes;
  public Matrix input, output;
  public Matrix[] hiddenLayers, synapses, biases;

  public NeuralNetwork(int inputs, int outputs, int numberOfHiddenLayers, 
                       int numberOfHiddenNodes)
  {
    this.inputs = inputs;
    this.outputs = outputs;
    this.numberOfHiddenLayers = numberOfHiddenLayers;
    this.numberOfHiddenNodes = numberOfHiddenNodes;

    input = new Matrix(inputs, 1);
    output = new Matrix(outputs, 1);

    hiddenLayers = new Matrix[numberOfHiddenLayers];
    synapses = new Matrix[numberOfHiddenLayers + 1];
    biases = new Matrix[numberOfHiddenLayers + 1];

    for (int i = 0; i < numberOfHiddenLayers; i++)
    {
      if (i == 0) { synapses[i] = new Matrix(numberOfHiddenNodes, inputs).Randomise(); }

      synapses[i] = (i == 0) ? new Matrix(numberOfHiddenNodes, inputs).Randomise() : new Matrix(numberOfHiddenNodes, numberOfHiddenNodes).Randomise();

      hiddenLayers[i] = new Matrix(numberOfHiddenNodes, 1);
      biases[i] = new Matrix(numberOfHiddenNodes, 1).Randomise();
    }

    synapses[numberOfHiddenLayers] = new Matrix(outputs, numberOfHiddenNodes).Randomise();
    biases[numberOfHiddenLayers] = new Matrix(outputs, 1).Randomise();
  }

  // Creating a neural network from the set of matrices
  public NeuralNetwork(Matrix input, Matrix output, Matrix[] hiddenLayers, 
                       Matrix[] synapses, Matrix[] biases)
  {
    // Setting all the counterparts of the neural network together
    this.input = input;
    this.output = output;
    this.hiddenLayers = hiddenLayers;
    this.synapses = synapses;
    this.biases = biases;


    // Getting the structure from the data values
    inputs = input.row;
    outputs = output.row;

    numberOfHiddenLayers = hiddenLayers.Length;
    numberOfHiddenNodes = hiddenLayers.Length < 0 ? hiddenLayers[0].row : 0;
  }

  public Matrix ForwardRun(Matrix input)
  {
    this.input = input;
    hiddenLayers[0] = Matrix.Sigmoid(synapses[0] * input + biases[0]);

    for (int i = 1; i < numberOfHiddenLayers; i++)
    {
      hiddenLayers[i] = Matrix.Sigmoid(synapses[i] * hiddenLayers[i - 1] + biases[i]);
    }

    output = synapses[numberOfHiddenLayers] * hiddenLayers[numberOfHiddenLayers - 1] + biases[numberOfHiddenLayers];
    return output;
  }

  public NeuralNetwork Copy()
  {
    return new NeuralNetwork(input.Copy(), output.Copy(), 
                             Matrix.Copy(hiddenLayers), Matrix.Copy(synapses), Matrix.Copy(biases));
  }
}