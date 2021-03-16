using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;

[assembly:UseReporter(typeof(NUnitReporter), typeof(WinMergeReporter))]