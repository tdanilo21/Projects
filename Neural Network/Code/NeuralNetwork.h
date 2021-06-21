#pragma once
#include <vector>
#include "Layer.h"

using namespace std;

/* TODO LIST:
 *	  Transpose function (Converts data into percentage).
 *    Input and output max data value should be handed in on the construction of object, not every time object is run.
 *    Use ToArray and FromArray functions from Matrix class.
 *    Back prop should be layer to layer function that class Layer owns.
 */

class NeuralNetwork
{
private:
	int size;
	vector<Layer> layers;

	void Resize(int size);
	void SetLayout(const vector<int>& layout);

	void Reset();
	void FeedForward(const Matrix<MyDouble>& input);
	Matrix<MyDouble>* GetOutput() const;
	void BackPropagate(const Matrix<MyDouble>& answer);
public:
	static constexpr double learning_rate = 0.15f;

	NeuralNetwork();
	NeuralNetwork(int size, const vector<int>& layout);

	vector<double>* Run(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData);
	vector<double>* Train(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData,
		const vector<double>& answer);
};
