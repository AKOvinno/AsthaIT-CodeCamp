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
Node* getMid(Node *head)
{
    int sz = 0;
    Node *tmp = head;
    while (tmp != NULL)
    {
        sz++;
        tmp = tmp->next;
    }
    int mid = (sz-1) / 2;
    tmp = head;
    for (int i = 0; i < mid; i++)
    {
        tmp = tmp->next;
    }
    return tmp;
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
bool isPalindrome(Node *head)
{
    if (head == NULL || head->next == NULL) return true;

    Node *mid = getMid(head);
    Node *secondHalf = reverse(mid->next);
    mid->next = NULL;

    Node *first = head;
    Node *second = secondHalf;
    bool result = true;

    while (second != NULL)
    {
        if (first->val != second->val)
        {
            result = false;
            break;
        }
        first = first->next;
        second = second->next;
    }
    mid->next = reverse(secondHalf);
    return result;
}
int main()
{
    int test;
    cin >> test;
    cin.ignore();
    while(test--)
    {
        char c;
        Node *head = NULL;
        Node *tail = NULL;
        while(cin.get(c) && c != '\n')
        {
            if(c != ' ')
            {
                insertAtTail(head, tail, c);
            }
        }
        if(isPalindrome(head)) cout << "Palindrome\n";
        else cout << "Not Palindrome\n";
    }
    return 0;
}

