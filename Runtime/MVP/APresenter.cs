namespace HM.CodeBase
{
    /// <summary>
    /// 프레젠터를 생성한다면 꼭 이 스크립트를 상속 요망
    /// </summary>
    public abstract class APresenter : IDispose, IOpen
    {
        public abstract void Dispose();

        public abstract void Open();

        public abstract void Close();
    }
}