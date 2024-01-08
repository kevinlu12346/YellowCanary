# Yellow Canary Code Challenge

## Objective

The task is to take in three pieces of data:
1) payslips for an employee (employee code)
2) payments (disbursements) made to a Super fund, and 
3) a set of instructions of how to treat each type of payment (Pay Code)

For a company to correctly pay super for an employee they must pay their staff 9.5% of Pay Codes that are treated as Ordinary Time Earnings (OTE) within 28 days after the end of the quarter. Quarters run from Jan-March, Apr-June, Jul-Sept, Oct-Dec.

Super earned between 1st Jan and the 31st of March (Q1) will need to be paid/disbursed between the 29th Jan - 28th of Apr.

For example:

Pay Codes are:
	- Salary = OTE
	- Site Allowance = OTE
	- Overtime - Weekend = Not OTE
	- Super Withheld = Not OTE
	
Payslips are:

    Jan 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Overtime - Weekend, Amount $1500
    	- Code: Super Withheld = $475

    Feb 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Super Withheld = $475

    March 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Super Withheld = $475


Disbursements:
- $500 on 27th Feb
- $500 on 30th March
- $500 on 30th Apr

For this Quarter we need to know the following:

	1) Total OTE = $15,000
	2) Total Super Payable = $1425
	3) Total Disbursed = $1000
	4) Variance = $425
	

## Challenge

Read in the sample file which contains the Disbursements, Payslips and Pay Codes.

For each of the 4 employees show the Quarterly grouping for the OTE amount, Super Payable and the Disbursement totals.

## Tips

Check out https://github.com/ExcelDataReader/ExcelDataReader for the Excel Data Reader

## Guidelines

* Add any assumptions you make about the problem and solution into a README file.
* While some companies ask candidates to build "Production Ready" solutions, we're looking for _pragmatic_ and _fit-for-purpose_ examples. For example, implementing unneeded patterns such as CQRS may impress some technical assessors, but in this case, would raise questions about why this approach has been taken.
* The solution should take between 2-3 hours. Time-box if you can't complete this challenge in a reasonable timeframe.
* Please submit the project in a git repository and share the link once completed.

