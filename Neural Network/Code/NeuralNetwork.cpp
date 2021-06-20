#include <vector>
#include <assert.h>
#include "LambdaFunctions.h"
#include "NeuralNetwork.h"

using namespace std;

NeuralNetwork::NeuralNetwork() {}
NeuralNetwork::NeuralNetwork(int size, const vector<int>& layout) {
	Resize(size);
	SetLayout(layout);
}


void NeuralNetwork::Resize(int size) { this->size = size; this->layers = *new vector<Layer>(size); }
void NeuralNetwork::SetLayout(const vector<int>& layout) {
	Resize(layout.size());
	for (int i = 0; i < this->size; i++) {
		int next_size = (i < this->size - 1 ? layout[i + 1] : 0);
		this->layers[i] = *new Layer(layout[i], next_size);
	}
}


void NeuralNetwork::Reset() { for (auto& l : this->layers) l.Reset(); }
void NeuralNetwork::FeedForward(const Matrix<MyDouble>& input) {
	this->layers[0].SetValues(input);
	for (int i = 0; i < this->size - 1; i++) {
		Matrix<MyDouble> next_values = *this->layers[i].Propagate();
		this->layers[i + 1].SetValues(next_values);
	}
}
Matrix<MyDouble>* NeuralNetwork::GetOutput() const { return new Matrix<MyDouble>(*this->layers.back().GetValues()); }
void NeuralNetwork::BackPropagate(const Matrix<MyDouble>& answer) {
	// Calculating errors for the last layer:
	vector<Matrix<MyDouble> > errors = *new vector<Matrix<MyDouble> >(this->size);
	errors.back() = *Matrix<MyDouble>::ElementWise(answer, *this->layers.back().GetValues(), Subtract);
	errors.back().Scale([](const MyDouble& x) { return x * x; });

	// Calculating derivatives for the last layer:
	vector<Matrix<MyDouble> > derivatives = *new vector<Matrix<MyDouble> >(this->size);
	derivatives.back() = *Matrix<MyDouble>::ElementWise(answer, *this->layers.back().GetValues(), Subtract);
	derivatives.back().Scale([](const MyDouble& x) { return *new MyDouble(2) * x; });

	// Back Propagating:
	for (int i = this->size - 2; i >= 0; i--) {
		Matrix<MyDouble> weights = *this->layers[i].GetWeights();
		Matrix<MyDouble> bias = *this->layers[i].GetBias();
		Matrix<MyDouble> previous_values_t = *Matrix<MyDouble>::Transpose(*this->layers[i].GetValues());
		Matrix<MyDouble> next_values = *this->layers[i + 1].GetValues();

		// Calculating gradiant descent:
		Matrix<MyDouble> gradient = *new Matrix<MyDouble>(next_values);
		gradient.Scale(ActivationFunctionDerivative);

		// Calculating delta weights:
		Matrix<MyDouble> delta_weights = *Matrix<MyDouble>::ElementWise(errors[i + 1], gradient, Multiply);
		delta_weights = *Matrix<MyDouble>::ElementWise(delta_weights, derivatives[i + 1], Multiply);
		delta_weights = *Matrix<MyDouble>::Product(delta_weights, previous_values_t);
		delta_weights.Scale([](const MyDouble& x) { return *new MyDouble(NeuralNetwork::learning_rate) * x; });
		delta_weights = *Matrix<MyDouble>::Transpose(delta_weights);

		// Adjusting weights:
		this->layers[i].SetWeights(*Matrix<MyDouble>::ElementWise(weights, delta_weights, Add));

		// Calculating delta biases:
		Matrix<MyDouble> delta_bias = *Matrix<MyDouble>::ElementWise(errors[i + 1], gradient, Multiply);
		delta_bias = *Matrix<MyDouble>::ElementWise(delta_bias, derivatives[i + 1], Multiply);
		delta_bias.Scale([](const MyDouble& x) { return *new MyDouble(NeuralNetwork::learning_rate) * x; });

		// Adjusting biases:
		this->layers[i].SetBias(*Matrix<MyDouble>::ElementWise(bias, delta_bias, Add));

		// Calculating derivatives:
		derivatives[i] = *Matrix<MyDouble>::ElementWise(derivatives[i + 1], gradient, Multiply);
		derivatives[i] = *Matrix<MyDouble>::Product(weights, derivatives[i]);

		// Calculating errors:
		errors[i] = *Matrix<MyDouble>::Product(weights, errors[i + 1]);
	}
}


vector<double>* NeuralNetwork::Run(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData) {
	assert(inputData.size() == this->layers[0].Size());
	assert(maxInputData.size() == this->layers[0].Size());
	assert(maxOutputData.size() == this->layers.back().Size());

	Matrix<MyDouble> input_mat = *new Matrix<MyDouble>(this->layers[0].Size());
	for (int i = 0; i < this->layers[0].Size(); i++)
		input_mat[i][0] = *new MyDouble(inputData[i] / maxInputData[i]);

	Reset();
	FeedForward(input_mat);
	Matrix<MyDouble> output_mat = *GetOutput();

	vector<double>* output = new vector<double>(this->layers.back().Size());
	for (int i = 0; i < this->layers.back().Size(); i++)
		(*output)[i] = output_mat[i][0].val * maxOutputData[i];
	return output;
}
vector<double>* NeuralNetwork::Train(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData,
	const vector<double>& answer) {

	assert(inputData.size() == this->layers[0].Size());
	assert(maxInputData.size() == this->layers[0].Size());
	assert(maxOutputData.size() == this->layers.back().Size());
	assert(answer.size() == this->layers.back().Size());

	vector<double>* output = Run(inputData, maxInputData, maxOutputData);

	Matrix<MyDouble> answer_mat = *new Matrix<MyDouble>(this->layers.back().Size());
	for (int i = 0; i < this->layers.back().Size(); i++)
		answer_mat[i][0] = *new MyDouble(answer[i] / maxOutputData[i]);
	BackPropagate(answer_mat);

	return output;
}
