using UniversityGameProject.Input;

namespace UniversityGameProject.Main.Timer;

public class Timer : Node
{
   private long _timeElapsed = 0;
   private bool _isStopped = true;

   public long Time => _timeElapsed;
   
   public Timer(string name) : base(name) { }

   public void Start()
   {
      _isStopped = false;
      _timeElapsed = 0;
   }

   public void Pause()
   {
      _isStopped = true;
   }

   public void Stop()
   {
      _isStopped = true;
   }

   public void Reset()
   {
      _timeElapsed = 0;
   }

   public bool IsActive()
   {
      if (!_isStopped)
      {
         return true;
      }

      return false;
   }

   public virtual void Update(long elapsedMilliseconds)
   {
      if (!_isStopped)
      {
         _timeElapsed += elapsedMilliseconds;
      }

      else
      {
         //Console.WriteLine("TimerStopped");
      }
   }
}
public class ReverseTimer : Node
{
   private long _timeElapsed = 0;
   private bool _isStopped = true;
   private long _duration;
   
   public ReverseTimer(string name) : base(name) { }

   public ReverseTimer(string name, long duration) : base(name)
   {
      _duration = duration;
      _timeElapsed = _duration;
   }
   
   public void Start()
   {
      _isStopped = false;
      _timeElapsed = _duration;
   }

   public void Pause()
   {
      _isStopped = true;
   }

   public void Stop()
   {
      _isStopped = true;
   }

   public virtual void Update(long elapsedMilliseconds)
   {
      if (!_isStopped)
      {
         _timeElapsed -= elapsedMilliseconds;
      }

      else
      {
         Console.WriteLine("TimerStopped");
      }
   }
}
