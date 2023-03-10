#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

#include "stm32f4xx_hal.h"

#define B1_Pin GPIO_PIN_13
#define B1_GPIO_Port GPIOC
#define B1_EXTI_IRQn EXTI15_10_IRQn
#define USART_TX_Pin GPIO_PIN_2
#define USART_TX_GPIO_Port GPIOA
#define USART_RX_Pin GPIO_PIN_3
#define USART_RX_GPIO_Port GPIOA
#define LD2_Pin GPIO_PIN_5
#define LD2_GPIO_Port GPIOA
#define SENSOR_ALL_GPIO_PORT GPIOB
#define SENSOR_LEFT_GPIO_PORT GPIOB
#define SENSOR_MID_GPIO_PORT GPIOB
#define SENSOR_RIGHT_GPIO_PORT GPIOB
#define SENSOR_LEFT_GPIO_PIN GPIO_PIN_3
#define SENSOR_MID_GPIO_PIN GPIO_PIN_4
#define SENSOR_RIGHT_GPIO_PIN GPIO_PIN_5

void HAL_TIM_MspPostInit(TIM_HandleTypeDef *htim);

void Motor_StartPwm();
void ReadSensor();
void Forward();
void Backward();
void TurnRight();
void TurnLeft();
void StopMove();
void RemoteControl();

void Error_Handler(void);

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */
