// package main

// import (
// 	"fmt"
// 	"math"
// 	"math/rand"
// 	"time"
// )

// func monteCarlo() float64 {
// 	n := 100000
// 	count := 0

// 	// Инициализация генератора случайных чисел только один раз
// 	rand.Seed(time.Now().UnixNano())

// 	for i := 0; i < n; i++ {
// 		// Генерация случайного числа от 0 до 3 и от 0 до 0.12
// 		random_x := rand.Float64() * 3.0
// 		random_y := rand.Float64() * 0.12

// 		if random_y < (math.Sin(random_x)*math.Sin(random_x))/(13-12*math.Cos(random_x)) {
// 			count++
// 		}
// 	}

// 	// Вычисление площади
// 	area := float64(count) / float64(n) * 3.0 * 0.12

// 	return area
// }

// func monte() {

// 	p := monteCarlo()
// 	fmt.Print("Приблизительное значение площади равно ", p, "\n")

// 	//формула погрешности
// 	k := 20 //проведем 20 опытов
// 	sumS1 := 0.0
// 	sumS2 := 0.0

// 	for i := 0; i < k; i++ {
// 		a := monteCarlo()
// 		sumS1 = sumS1 + float64(a)
// 		sumS2 = sumS2 + math.Pow(float64(a), 2)
// 	}

// 	sumS2 = sumS2 / float64(k)
// 	sumS1 = math.Pow(sumS1/float64(k), 2)

// 	sigma := math.Sqrt(sumS2 - sumS1)
// 	fmt.Print("Погрешность равна ", sigma)
// }
