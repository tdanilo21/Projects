#pragma once
#include <vector>
#include "MyDouble.h"
#include "Matrix.h"
#include "Layer.h"

using namespace std;

/* TODO LIST:
 *    Use ToArray and FromArray functions from Matrix class.
 *    Back prop should be layer to layer function that class Layer owns.
 */

class NeuralNetwork
{
private:
	int size;
	vector<Layer> layers;
	Matrix<MyDouble> maxInput, maxOutput;

	void Resize(int size);
	void SetLayout(const vector<int>& layout);
	void SetMaxData(const vector<double>& maxInputData, const vector<double>& maxOutputData);

	void Reset();
	void FeedForward(const Matrix<MyDouble>& input);
	Matrix<MyDouble>* GetOutput() const;
	void BackPropagate(const Matrix<MyDouble>& answer);
public:
	static constexpr double learning_rate = 0.15f;

	NeuralNetwork();
	NeuralNetwork(int size, const vector<int>& layout, const vector<double>& maxInputData, const vector<double>& maxOutputData);

	vector<double>* Run(const vector<double>& inputData);
	vector<double>* Train(const vector<double>& inputData, const vector<double>& answer);
};
