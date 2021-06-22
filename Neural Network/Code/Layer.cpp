#include <assert.h>
#include "LambdaFunctions.h"
#include "MyDouble.h"
#include "Matrix.h"
#include "Layer.h"

using namespace std;

Layer::Layer() {}
Layer::Layer(int size, int next_size) { Resize(size, next_size); }


void Layer::Resize(int size, int next_size) {
	this->size = size;
	this->next_size = next_size;
	this->values = Matrix<MyDouble>(size);
	this->weights = Matrix<MyDouble>(size, next_size);
	this->weights.Randomize();
	this->bias = Matrix<MyDouble>(next_size);
	this->bias.Randomize();
}
int Layer::Size() const { return this->size; }

void Layer::SetValues(const Matrix<MyDouble>& new_values) {
	assert(new_values.rows == this->size && new_values.cols == 1);
	this->values = Matrix<MyDouble>(new_values);
}
Matrix<MyDouble>* Layer::GetValues() const { return new Matrix<MyDouble>(this->values); }

void Layer::SetWeights(const Matrix<MyDouble>& new_weights) {
	assert(new_weights.rows == this->size && new_weights.cols == this->next_size);
	this->weights = Matrix<MyDouble>(new_weights);
}
Matrix<MyDouble>* Layer::GetWeights() const { return new Matrix<MyDouble>(this->weights); }

void Layer::SetBias(const Matrix<MyDouble>& new_bias) {
	assert(new_bias.rows == this->next_size && new_bias.cols == 1);
	this->bias = Matrix<MyDouble>(new_bias);
}
Matrix<MyDouble>* Layer::GetBias() const { return new Matrix<MyDouble>(this->bias); }


Matrix<MyDouble>* Layer::Propagate() const {
	Matrix<MyDouble>* weights_t = Matrix<MyDouble>::Transpose(this->weights);
	Matrix<MyDouble>* next_values = Matrix<MyDouble>::Product(*weights_t, this->values);
	next_values = Matrix<MyDouble>::ElementWise(*next_values, this->bias, Add);
	next_values->Scale(ActivationFunction);

	delete weights_t;
	return next_values;
}
void Layer::Reset() { this->values = Matrix<MyDouble>(size); }
