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
    int n;
    Node *head = NULL;
    Node *tail = NULL;
    while(cin >> n)
    {
        insertAtTail(head, tail, n);
    }
    int sz = Size(head);
    int first_half = sz/2;
    Node *tmp = head;
    for(int i = 0; i < first_half; i++)
    {
        cout << tmp->val << " ";
        tmp = tmp->next;
    }
    Node *second_half = reverse(tmp);
    Node *cur = second_half;
    while(cur!=NULL)
    {
        cout << cur->val;
        if(cur->next != NULL) cout << " ";
        cur = cur->next;
    }
    cout << "\n";
    return 0;
}
