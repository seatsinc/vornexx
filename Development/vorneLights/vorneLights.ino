
#define BLUE_LED 5
#define RED_LED 6
#define GREEN_LED 9

int red = 0;
int green = 0;
int blue = 0;

char c = 'x';

void setup() {
  // put your setup code here, to run once:

  

  delay(3000);

  Serial.begin(9600);
  Serial.setTimeout(1000);

  pinMode(GREEN_LED, OUTPUT);
  pinMode(RED_LED, OUTPUT);
  pinMode(BLUE_LED, OUTPUT);


}

void loop() {
  // put your main code here, to run repeatedly:

  
  
  if (Serial.available()) {
    c = (char)Serial.read();

    if (c == 'r') {
      red = 255;
      green = 0;
      blue = 0;
    }
    else if (c == 'g') {
      red = 0;
      green = 255;
      blue = 0;
    }
    else if (c == 'y') {
      red = 255;
      green = 128;
      blue = 0;
    }
    else if (c == 'b') {
      red = 0;
      green = 0;
      blue = 255;
    }
    else if (c == 'w') {
      red = 255;
      green = 255;
      blue = 255;
    }
    else {
      red = 0;
      green = 0;
      blue = 0;
    }

    writeColor(red, green, blue);

    Serial.write(c);

      
  }

  delay(250);

}

void writeColor(int r, int g, int b) {

  analogWrite(RED_LED, r);
  analogWrite(GREEN_LED, g);
  analogWrite(BLUE_LED, b);
  
}
