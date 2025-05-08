with Ada.Text_IO; use Ada.Text_IO;
with Ada.Integer_Text_IO; use Ada.Integer_Text_IO;

procedure Main is

   Max_Buffer_Size : constant := 5;
   Total_Products  : constant := 15;

   subtype Item_Type is Integer;
   type Buffer_Type is array (1 .. Max_Buffer_Size) of Item_Type;

   protected type Monitor is
      entry Produce(Item : in Item_Type);
      entry Consume(Item : out Item_Type);
   private
      Buffer       : Buffer_Type;
      In_Index     : Integer := 1;
      Out_Index    : Integer := 1;
      Count        : Integer := 0;
   end Monitor;

   protected body Monitor is
      entry Produce(Item : in Item_Type) when Count < Max_Buffer_Size is
      begin
         Buffer(In_Index) := Item;
         In_Index := (In_Index mod Max_Buffer_Size) + 1;
         Count := Count + 1;
      end Produce;

      entry Consume(Item : out Item_Type) when Count > 0 is
      begin
         Item := Buffer(Out_Index);
         Out_Index := (Out_Index mod Max_Buffer_Size) + 1;
         Count := Count - 1;
      end Consume;
   end Monitor;

   Shared_Buffer : Monitor;

   task type Producer(Id : Integer; Items_To_Produce : Integer);
   task body Producer is
   begin
      for I in 1 .. Items_To_Produce loop
         Shared_Buffer.Produce(I);
         Put_Line("Producer " & Id'Image & " produced item " & I'Image);
      end loop;
   end Producer;

   task type Consumer(Id : Integer; Items_To_Consume : Integer);
   task body Consumer is
      Item : Item_Type;
   begin
      for I in 1 .. Items_To_Consume loop
         Shared_Buffer.Consume(Item);
         Put_Line("Consumer " & Id'Image & " consumed item " & Item'Image);
      end loop;
   end Consumer;


   P1 : Producer(1, 5);
   P2 : Producer(2, 5);
   P3 : Producer(3, 5);

   C1 : Consumer(1, 7);
   C2 : Consumer(2, 8);

begin
   null;
end Main;
