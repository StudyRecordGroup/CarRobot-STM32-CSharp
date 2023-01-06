#include "main.h"

#define BLACK 0
#define WHITE 1
#define REMOTE_CONTROL 0
#define LINE_FOLOW 1
#define FRONT 0 
#define BACK 1
#define FULL_PWM_CAPTURE 1000
#define CAN_MOVE_SPEED 700
#define MOVE_SPEED_L 375
#define MOVE_SPEED_R 375
#define NONE_SPEED 0

uint8_t mode;
uint8_t direction = FRONT;

TIM_HandleTypeDef htim3;
TIM_HandleTypeDef htim4;

UART_HandleTypeDef huart2;

void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_TIM3_Init(void);
static void MX_TIM4_Init(void);
static void MX_USART2_UART_Init(void);

void HAL_GPIO_EXTI_Callback(uint16_t GPIO_Pin)
{
	if(GPIO_Pin == GPIO_PIN_13)
	{
		mode = (mode == 0)? 1:0;
	}
  HAL_GPIO_WritePin(LD2_GPIO_Port, LD2_Pin, mode^1);
  HAL_GPIO_EXTI_IRQHandler(GPIO_PIN_13);
}

int main(void)
{
  HAL_Init();

  SystemClock_Config();

  MX_GPIO_Init();
  MX_TIM3_Init();
  MX_TIM4_Init();
  MX_USART2_UART_Init();
  Motor_StartPwm();

  while (1)
  {
    if(mode==REMOTE_CONTROL){
      StopMove();
      RemoteControl();
    }
    else{
      ReadSensor();
    }
      
  }

}

void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Configure the main internal regulator output voltage
  */
  __HAL_RCC_PWR_CLK_ENABLE();
  __HAL_PWR_VOLTAGESCALING_CONFIG(PWR_REGULATOR_VOLTAGE_SCALE2);

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSI;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.HSICalibrationValue = RCC_HSICALIBRATION_DEFAULT;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSI;
  RCC_OscInitStruct.PLL.PLLM = 8;
  RCC_OscInitStruct.PLL.PLLN = 64;
  RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
  RCC_OscInitStruct.PLL.PLLQ = 7;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }

  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV2;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV1;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_1) != HAL_OK)
  {
    Error_Handler();
  }
}

static void MX_TIM3_Init(void)
{

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};
  TIM_OC_InitTypeDef sConfigOC = {0};

  htim3.Instance = TIM3;
  htim3.Init.Prescaler = 64;
  htim3.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim3.Init.Period = 1000;
  htim3.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
  htim3.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
  if (HAL_TIM_Base_Init(&htim3) != HAL_OK)
  {
    Error_Handler();
  }
  sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
  if (HAL_TIM_ConfigClockSource(&htim3, &sClockSourceConfig) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_Init(&htim3) != HAL_OK)
  {
    Error_Handler();
  }
  sMasterConfig.MasterOutputTrigger = TIM_TRGO_RESET;
  sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
  if (HAL_TIMEx_MasterConfigSynchronization(&htim3, &sMasterConfig) != HAL_OK)
  {
    Error_Handler();
  }
  sConfigOC.OCMode = TIM_OCMODE_PWM1;
  sConfigOC.Pulse = 0;
  sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
  sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
  if (HAL_TIM_PWM_ConfigChannel(&htim3, &sConfigOC, TIM_CHANNEL_1) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_ConfigChannel(&htim3, &sConfigOC, TIM_CHANNEL_2) != HAL_OK)
  {
    Error_Handler();
  }

  HAL_TIM_MspPostInit(&htim3);

}


static void MX_TIM4_Init(void)
{

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};
  TIM_OC_InitTypeDef sConfigOC = {0};

  htim4.Instance = TIM4;
  htim4.Init.Prescaler = 64;
  htim4.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim4.Init.Period = 1000;
  htim4.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
  htim4.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
  if (HAL_TIM_Base_Init(&htim4) != HAL_OK)
  {
    Error_Handler();
  }
  sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
  if (HAL_TIM_ConfigClockSource(&htim4, &sClockSourceConfig) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_Init(&htim4) != HAL_OK)
  {
    Error_Handler();
  }
  sMasterConfig.MasterOutputTrigger = TIM_TRGO_RESET;
  sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
  if (HAL_TIMEx_MasterConfigSynchronization(&htim4, &sMasterConfig) != HAL_OK)
  {
    Error_Handler();
  }
  sConfigOC.OCMode = TIM_OCMODE_PWM1;
  sConfigOC.Pulse = 0;
  sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
  sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
  if (HAL_TIM_PWM_ConfigChannel(&htim4, &sConfigOC, TIM_CHANNEL_1) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_PWM_ConfigChannel(&htim4, &sConfigOC, TIM_CHANNEL_2) != HAL_OK)
  {
    Error_Handler();
  }

  HAL_TIM_MspPostInit(&htim4);

}

static void MX_USART2_UART_Init(void)
{

  huart2.Instance = USART2;
  huart2.Init.BaudRate = 115200;
  huart2.Init.WordLength = UART_WORDLENGTH_8B;
  huart2.Init.StopBits = UART_STOPBITS_1;
  huart2.Init.Parity = UART_PARITY_NONE;
  huart2.Init.Mode = UART_MODE_TX_RX;
  huart2.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart2.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart2) != HAL_OK)
  {
    Error_Handler();
  }

}

static void MX_GPIO_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStruct = {0};

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOH_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(LD2_GPIO_Port, LD2_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin : B1_Pin */
  GPIO_InitStruct.Pin = B1_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_FALLING;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(B1_GPIO_Port, &GPIO_InitStruct);

  /*Configure GPIO pin : LD2_Pin */
  GPIO_InitStruct.Pin = LD2_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(LD2_GPIO_Port, &GPIO_InitStruct);

  /*Configure GPIO pins : PB3 PB4 PB5 */
  GPIO_InitStruct.Pin = GPIO_PIN_3|GPIO_PIN_4|GPIO_PIN_5;
  GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

  /* EXTI interrupt init*/
  HAL_NVIC_SetPriority(EXTI15_10_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(EXTI15_10_IRQn);

  HAL_GPIO_WritePin(LD2_GPIO_Port, LD2_Pin, mode^1);
}

void Motor_StartPwm()
{
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_1);
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_2);
  HAL_TIM_PWM_Start(&htim4, TIM_CHANNEL_1);
  HAL_TIM_PWM_Start(&htim4, TIM_CHANNEL_2);

  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, FULL_PWM_CAPTURE);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, FULL_PWM_CAPTURE);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, FULL_PWM_CAPTURE);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, FULL_PWM_CAPTURE);
}

void ReadSensor()
{
  GPIO_PinState left = HAL_GPIO_ReadPin(SENSOR_LEFT_GPIO_PORT, SENSOR_LEFT_GPIO_PIN);
  GPIO_PinState mid = HAL_GPIO_ReadPin(SENSOR_MID_GPIO_PORT, SENSOR_MID_GPIO_PIN);
  GPIO_PinState right = HAL_GPIO_ReadPin(SENSOR_RIGHT_GPIO_PORT, SENSOR_RIGHT_GPIO_PIN);

  if (left == WHITE && mid == WHITE && right == WHITE)
  {
    Forward();
    //Backward();
  }
  else if (left == WHITE && mid == WHITE && right == BLACK)
  {
    TurnRight();
  }
  else if (left == WHITE && mid == BLACK && right == BLACK)
  {
    TurnRight();
  }
  else if (left == WHITE && mid == BLACK && right == WHITE)
  {
    Forward();
  }
  else if (left == BLACK && mid == WHITE && right == WHITE)
  {
    TurnLeft();
  }
  else if (left == BLACK && mid == BLACK && right == WHITE)
  {
    TurnLeft();
  }
  else if (left == BLACK && mid == WHITE && right == BLACK)
  {
    Forward();
  }
  else
  {
    Forward();
  }
}

void delay(uint32_t count)
{
  while(CAN_MOVE_SPEED - (count++));
}

void Forward()
{
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, CAN_MOVE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, CAN_MOVE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
  delay(MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, MOVE_SPEED_R);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
}

void Backward()
{
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, CAN_MOVE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, CAN_MOVE_SPEED);
  delay(MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, MOVE_SPEED_R);
}

void TurnRight()
{
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, CAN_MOVE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
  delay(MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
}

void TurnLeft()
{
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, CAN_MOVE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
  delay(MOVE_SPEED_L);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, MOVE_SPEED_R);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
}

void StopMove()
{
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim3, TIM_CHANNEL_2, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_1, NONE_SPEED);
  __HAL_TIM_SetCompare(&htim4, TIM_CHANNEL_2, NONE_SPEED);
}

void RemoteControl()
{

}

void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */
