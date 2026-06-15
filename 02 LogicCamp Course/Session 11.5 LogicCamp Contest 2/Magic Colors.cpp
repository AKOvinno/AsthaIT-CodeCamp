#include<bits/stdc++.h>
using namespace std;

char mix(char a, char b)
{
    if((a=='R'&&b=='B')||(a=='B'&&b=='R')) return 'P';
    if((a=='R'&&b=='G')||(a=='G'&&b=='R')) return 'Y';
    if((a=='B'&&b=='G')||(a=='G'&&b=='B')) return 'C';
    return '\0';
}

bool canMix(char a, char b)
{
    if(a == b) return true;
    char r = mix(a, b);
    return r != '\0';
}

int main()
{
    int t;
    cin >> t;

    while(t--)
    {
        int n;
        cin >> n;
        string colors;
        cin >> colors;

        stack<char> st;

        for(int i = 0; i < n; i++)
        {
            st.push(colors[i]);

            bool reacted = true;
            while(reacted && st.size() >= 2)
            {
                reacted = false;
                char top = st.top();
                st.pop();
                char second = st.top();
                st.pop();

                if(top == second)
                {
                    reacted = true;
                }
                else
                {
                    char mixed = mix(top, second);
                    if(mixed != '\0')
                    {
                        st.push(mixed);
                        reacted = true;
                    }
                    else
                    {
                        st.push(second);
                        st.push(top);
                    }
                }
            }
        }
        string result = "";
        while(!st.empty())
        {
            result = st.top() + result;
            st.pop();
        }
        cout << result << "\n";
    }

    return 0;
}
