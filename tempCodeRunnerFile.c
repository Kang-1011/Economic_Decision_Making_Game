#include <stdio.h>
#include <stdbool.h>

bool is_prime(int num) {
    if (num <= 1) {
        return false;
    }

    for (int i = 2; i * i <= num; i++) {
        if (num % i == 0) {
            return false;
        }
    }

    return true;
}

int main() {
    int limit = 10000;
    long long sum = 0;

    for (int i = 2; i < limit; i++) {
        if (is_prime(i)) {
            sum += i;
        }
    }

    printf("The sum of all prime numbers below %d is: %lld\n", limit, sum);

    return 0;
}
