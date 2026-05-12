#include<bits/stdc++.h>
using namespace std;
class Node
{
public:
    int val;
    Node *next;
    Node(int val)
    {
        this->val = val;
        this->next = NULL;
    }
};
void insertAtTail(Node *&head, Node *&tail, int val)
{
    Node *newNode = new Node(val);
    if (head == NULL)
    {
        head = newNode;
        tail = newNode;
    }
    else
    {
        tail->next = newNode;
        tail = tail->next;
    }
}
int Size(Node *head)
{
    int sz = 0;
    while(head != NULL)
    {
        sz++;
        head = head->next;
    }
    return sz;
}
Node* reverse(Node *head)
{
    if (head == NULL || head->next == NULL)
    {
        return head;
    }
    Node* tmp = head->next;
    Node* newHead = reverse(head->next);
    tmp->next = head;
    head->next = NULL;
    return newHead;
}
int main()
{
    int test;
    cin >> test;
    cin.ignore();
    while(test--)
    {
        int n;
        Node *head = NULL;
        Node *tail = NULL;
        string line;
        getline(cin, line);
        istringstream input(line);
        while(input >> n)
        {
            insertAtTail(head, tail, n);
        }
        Node *cur = head;
        int swappable = 0;
        while(cur->next != NULL)
        {
            if(cur->next == NULL) break;
            if(cur->val > cur->next->val) swappable++;
            cur = cur->next;
        }
        if(swappable > 2) cout << "No\n";
        else cout << "Yes\n";
    }
    return 0;
}
