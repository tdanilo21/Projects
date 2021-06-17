#include <vector>
#include <assert.h>
#include "Matrix.h"
#include "Layer.h"

using namespace std;

MyDouble::MyDouble(double val) { this->val = val; }
MyDouble::MyDouble(const MyDouble& other) { *this = other; }
MyDouble& MyDouble::Random() { return *new MyDouble(((double)rand() / (double)RAND_MAX) * 2 - 1); }
MyDouble& MyDouble::DotProduct(const vector<MyDouble>& a, const vector<MyDouble>& b) {
	assert(a.size() == b.size());
	MyDouble result = *new MyDouble();
	for (int i = 0; i < a.size(); i++)
		result += a[i] * b[i];
	return result;
}
MyDouble& MyDouble::operator=(const MyDouble& other) { this->val = other.val; return *this; }
MyDouble& MyDouble::operator+=(const MyDouble& other) { this->val += other.val; return *this; }
MyDouble& MyDouble::operator*(const MyDouble& other) const { return *new MyDouble(this->val * other.val); }
MyDouble& MyDouble::operator+(const MyDouble& other) const { return *new MyDouble(this->val + other.val); }


// ************************ Lambda functions ************************ //

MyDouble ActivationFunction(const MyDouble& x) { return *new MyDouble(tanh(x.val)); }

MyDouble Add(const MyDouble& a, const MyDouble& b) { return a + b; }


// ********************** Layer ********************** //

Layer::Layer(int size, int next_size) { Resize(size, next_size); }

void Layer::Resize(int size, int next_size) {
	this->size = size;
	this->next_size = next_size;
	this->values = *new Matrix<MyDouble>(size);
	this->weights = *new Matrix<MyDouble>(size, next_size);
	this->weights.Randomize();
	this->bias = *new Matrix<MyDouble>(next_size);
	this->bias.Randomize();
}
int Layer::Size() const { return this->size; }

void Layer::SetValues(const Matrix<MyDouble>& new_values) {
	assert(new_values.rows == this->size && new_values.cols == 1);
	this->values = *new Matrix<MyDouble>(new_values);
}
Matrix<MyDouble>& Layer::GetValues() const { return *new Matrix<MyDouble>(this->values); }

void Layer::SetWeights(const Matrix<MyDouble>& new_weights) {
	assert(new_weights.rows == this->size && new_weights.cols == this->next_size);
	this->weights = *new Matrix<MyDouble>(new_weights);
}
Matrix<MyDouble>& Layer::GetWeights() const { return *new Matrix<MyDouble>(this->weights); }

void Layer::SetBias(const Matrix<MyDouble>& new_bias) {
	assert(new_bias.rows == this->next_size && new_bias.cols == 1);
	this->bias = *new Matrix<MyDouble>(new_bias);
}
Matrix<MyDouble>& Layer::GetBias() const { return *new Matrix<MyDouble>(this->bias); }

Matrix<MyDouble>& Layer::Propagate() const {
	Matrix<MyDouble> weights_t = Matrix<MyDouble>::Transpose(this->weights);
	Matrix<MyDouble> next_values = Matrix<MyDouble>::Product(weights_t, this->values);
	next_values = Matrix<MyDouble>::ElementWise(next_values, this->bias, Add);
	next_values.Scale(ActivationFunction);
	return next_values;
}
void Layer::Reset() { this->values = *new Matrix<MyDouble>(size); }
