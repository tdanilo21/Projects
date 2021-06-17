#include <vector>
#include <assert.h>
#include "Matrix.h"
#include "Layer.h"
#include "NeuralNetwork.h"

using namespace std;


// ************************ Lambda functions ************************ //



MyDouble ActivationFunction(const MyDouble& x) { return *new MyDouble(tanh(x.val)); }
MyDouble ActivationFunctionDerivative(const MyDouble& x) { return MyDouble(1.0f - x.val * x.val); }

MyDouble Multiply(const MyDouble& a, const MyDouble& b) { return a * b; }
MyDouble Add(const MyDouble& a, const MyDouble& b) { return a + b; }


// ************************ Neural Network ************************ //



NeuralNetwork::NeuralNetwork(){}
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
		Matrix<MyDouble> next_values = this->layers[i].Propagate();
		this->layers[i + 1].SetValues(next_values);
	}
}
Matrix<MyDouble>& NeuralNetwork::GetOutput() const { return *new Matrix<MyDouble>(this->layers.back().GetValues()); }
void NeuralNetwork::BackPropagate(const Matrix<MyDouble>& answer) {
	// TODO: BackProp
}


vector<double>& NeuralNetwork::Run(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData) {
	assert(inputData.size() == this->layers[0].Size());
	assert(maxInputData.size() == this->layers[0].Size());
	assert(maxOutputData.size() == this->layers.back().Size());

	Matrix<MyDouble> input_mat = *new Matrix<MyDouble>(this->layers[0].Size());
	for (int i = 0; i < this->layers[0].Size(); i++)
		input_mat[i][0] = *new MyDouble(inputData[i] / maxInputData[i]);

	Reset();
	FeedForward(input_mat);
	Matrix<MyDouble> output_mat = GetOutput();

	vector<double> output = *new vector<double>(this->layers.back().Size());
	for (int i = 0; i < this->layers.back().Size(); i++)
		output[i] = output_mat[i][0].val * maxOutputData[i];
	return output;
}
vector<double>& NeuralNetwork::Train(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData,
	const vector<double>& answer) {

	assert(inputData.size() == this->layers[0].Size());
	assert(maxInputData.size() == this->layers[0].Size());
	assert(maxOutputData.size() == this->layers.back().Size());
	assert(answer.size() == this->layers.back().Size());

	vector<double> output = Run(inputData, maxInputData, maxOutputData);

	Matrix<MyDouble> answer_mat = *new Matrix<MyDouble>(this->layers.back().Size());
	for (int i = 0; i < this->layers.back().Size(); i++)
		answer_mat[i][0] = *new MyDouble(answer[i] / maxOutputData[i]);
	BackPropagate(answer_mat);

	return output;
}
