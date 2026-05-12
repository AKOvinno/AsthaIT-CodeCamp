#include<bits/stdc++.h>
using namespace std;

int main()
{
    cout << "---------------- Normal Variable a --------------------\n";
    double a = 10.5;
    cout << "Address of a = " << &a << "\n";
    cout << "Value of a = " << a << "\n";
    cout << "---------------- 1st pointer ptr --------------------\n";
    double *ptr = &a;
    cout << "Printing first pointer = " << ptr << "\n";
    cout << "Address of first pointer = " << &ptr << "\n";
    cout << "Value of first pointer  = " << *ptr << "\n";
    cout << "---------------- 2nd pointer ptr_a --------------------\n";
    double **ptr_a = &ptr;
    cout << "Printing second pointer = " << ptr_a << "\n";
    cout << "Address of second pointer = " << &ptr_a << "\n";
    cout << "Value of second pointer = " << *ptr_a << "\n";
    cout << "Value of second pointer & first pointer = " << **ptr_a << "\n";
    return 0;
}

