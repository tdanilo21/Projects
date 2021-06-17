#pragma once
#include <vector>
#include "Layer.h"

using namespace std;

class NeuralNetwork
{
private:
	static constexpr double learning_rate = 0.15f;
	int size;
	vector<Layer> layers;

	void Resize(int size);
	void SetLayout(const vector<int>& layout);

	void Reset();
	void FeedForward(const Matrix<MyDouble>& input);
	Matrix<MyDouble>& GetOutput() const;
	void BackPropagate(const Matrix<MyDouble>& answer);
public:
	NeuralNetwork();
	NeuralNetwork(int size, const vector<int>& layout);

	vector<double>& Run(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData);
	vector<double>& Train(const vector<double>& inputData, const vector<double>& maxInputData, const vector<double>& maxOutputData,
		const vector<double>& answer);
};
