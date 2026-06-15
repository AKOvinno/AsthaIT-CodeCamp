#include<bits/stdc++.h>
using namespace std;

int main()
{
    long long int y;
    cin >> y;
    if( y == 1) cout << 1 << "\n";
    else {
        long long int result = pow(2, 2 * y - 3);
        cout << result << "\n";
    }
    return 0;
}
